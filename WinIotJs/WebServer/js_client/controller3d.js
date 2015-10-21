define(function () {

    var me;
    var container, camera, scene, renderer, controls;
    var arduino, group;

    function Controller3D(debugWriter, sprintf) {
        me = this;
        me.debugWriter = debugWriter;
        me.sprintf = sprintf;

        if (!(this instanceof Controller3D)) {
            throw new TypeError("Controller3D constructor cannot be called as a function.");
        }
    }

    Controller3D.prototype = {
        constructor: Controller3D,
        start: function() {
            var loader = new THREE.ColladaLoader();
            loader.options.convertUpAxis = true;
            loader.load( './raspberry.dae', function (collada) {
                arduino = collada.scene;
                arduino.scale.x = arduino.scale.y = arduino.scale.z = 3;
                arduino.updateMatrix();
                init();
                animate();
            });
        },
        useSensorData: function(data)
        {
            // Projection of the UP direction on axes
            var upVector = new THREE.Vector3(data.acc.x, data.acc.y, data.acc.z);

            // Projection of the magnetic field direction on axes
            var magVector = new THREE.Vector3(data.mag.x, data.mag.y, data.mag.z);

            rotateModel(upVector, magVector);
        }
    };

    function init() {
        container = document.getElementById("container3d");
        scene = new THREE.Scene();
        renderer = new THREE.WebGLRenderer();
        renderer.setSize(container.offsetWidth, container.offsetHeight);
        container.appendChild(renderer.domElement);

        // objects
        group = new THREE.Object3D();//create an empty container
        group.add( arduino);//add a mesh with geometry to it
        scene.add( group );
        
        /*
        // Mark axes
        var geometryX = new THREE.BoxGeometry(10, 10, 10 );
        var materialX = new THREE.MeshBasicMaterial( { color: 0xff0000 } );
        var cubeX = new THREE.Mesh( geometryX, materialX );
        cubeX.position.x = 200;
        group.add( cubeX );

        var geometryY = new THREE.BoxGeometry(10, 10, 10 );
        var materialY = new THREE.MeshBasicMaterial( { color: 0x00ff00 } );
        var cubeY = new THREE.Mesh( geometryY, materialY );
        cubeY.position.y = 200;
        group.add( cubeY );

        var geometryZ = new THREE.BoxGeometry(10, 10, 10 );
        var materialZ = new THREE.MeshBasicMaterial( { color: 0x0000ff } );
        var cubeZ = new THREE.Mesh( geometryZ, materialZ );
        cubeZ.position.z = 200;
        group.add( cubeZ );
         */ 

        // Lights
        var directionalLight = new THREE.DirectionalLight( 0xffffff, 1 );
        directionalLight.position.set( -1, 1, 1 );
        scene.add( directionalLight );

        var directionalLight2 = new THREE.DirectionalLight( 0xffffff, 0.2 );
        directionalLight2.position.set( 0, -1, 0 );
        scene.add( directionalLight2 );

        var directionalLight3 = new THREE.DirectionalLight( 0xffffff, 0.3 );
        directionalLight3.position.set( 1, 1, 0 );
        scene.add( directionalLight3 );

        // Camera
        var xSize = 250;
        var ySize = 250;
        if ( container.offsetWidth > container.offsetHeight)
        {
            xSize = ySize * container.offsetWidth / container.offsetHeight;
        }
        else
        {
            ySize = xSize * container.offsetHeight / container.offsetWidth;
        }

        camera = new THREE.OrthographicCamera( -xSize, xSize, ySize, -ySize, 1, 1000 );

        // Browser orientation controls
        controls = new DeviceOrientationController( camera, renderer.domElement );
        controls.connect();
    }

    function animate() {
        controls.update();

        var vector =   new THREE.Vector3(0, 0, -300);
        var m = camera.matrixWorld;
        vector.applyMatrix4(m);

        group.position.x = vector.x;
        group.position.y = vector.y;
        group.position.z = vector.z;

        requestAnimationFrame(animate);
        render();
    }

    function render() {
        //showCameraMatrix(camera);
        renderer.render(scene, camera);
    }

    function showCameraMatrix(camera)
    {
        var matrix = camera.matrixWorldInverse;

        var s = "";

        var p = sprintf("%.3f %.3f %.3f", camera.position.x, camera.position.y,camera.position.z);
        me.debugWriter.show("p", p);

        var a = sprintf("%.3f %.3f %.3f", arduino.position.x, arduino.position.y,arduino.position.z);
        me.debugWriter.show("a", a);

        try {
            for (var i = 0; i < 16; i++)
            {
                s += me.sprintf("%.3f", matrix.elements[i]);
                if (i%4 == 3)
                {
                    me.debugWriter.show("m" + i / 4, s);
                    s = "";
                }
                else
                {
                    s += " ";
                }
            }
        }
        catch(err)
        {
            toastr.error(err);
        }
    }

    function rotateModel(yProjection, xProjection)
    {
        yProjection.normalize();
        yProjection.normalize();

        // calculate third (X) vector
        var zProjection = new THREE.Vector3();
        zProjection.crossVectors(xProjection, yProjection);
        zProjection.normalize();

        // make X vector perpendicular to Y, Z
        xProjection.crossVectors(yProjection, zProjection);
        xProjection.normalize();

        var rotationMatrix = new THREE.Matrix4();
        rotationMatrix.elements[0] = xProjection.x;
        rotationMatrix.elements[4] = xProjection.y;
        rotationMatrix.elements[8] = xProjection.z;
        rotationMatrix.elements[1] = yProjection.x;
        rotationMatrix.elements[5] = yProjection.y;
        rotationMatrix.elements[9] = yProjection.z;
        rotationMatrix.elements[2] = zProjection.x;
        rotationMatrix.elements[6] = zProjection.y;
        rotationMatrix.elements[10] = zProjection.z;

        var d = rotationMatrix.determinant();

        var compensateGeo = new THREE.Matrix4();
        
        // Magnetic declination in Dachau, Germany is +2.49'
        compensateGeo.makeRotationY(  (180 - 3) * Math.PI / 180.0);


        compensateGeo.multiply(rotationMatrix);

        applyRotationMatrix(compensateGeo);
    }

    function applyRotationMatrix(rotationMatrix)
    {
        if (group === undefined)
        {
            // scene is not loaded yet
            return;
        }
        group.quaternion.setFromRotationMatrix(rotationMatrix);
    }
    return Controller3D;
});