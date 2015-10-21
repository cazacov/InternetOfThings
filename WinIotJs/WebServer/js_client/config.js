requirejs.config({
    baseUrl: "js_client",
    //urlArgs: "bust=" + (new Date()).getTime(),  // prevent caching
    shim: {
    },
    paths: {
        sprintf: "lib/sprintf/dist/sprintf.min",
        requirejs: "lib/requirejs/require",
    }
});