var client = new Messaging.Client("192.168.178.48", 8000, "dashboard");
$(document).ready(function () {

  var gauge = new JustGage({
    id: "gauge",
    value: 10,
    min: 10,
    max: 30,
    title: "Temperature",
    label: "Celsius",
    levelColors: ["#0000FF", "#00A0A0", "#00FF00", "#A0A000", "#FF0000"],
    levelColorsGradient: false,
    refreshAnimationTime: 400
  });

  var messageCount = 0;
  var messagesPerSecond = 0;

  var options = {
    timeout: 3,
    onSuccess: function () {
      client.subscribe("home/status");
      client.subscribe("home/temperature");
    },
    onFailure: function (message) {
      alert("Connection failed");
    }
  };

  var temperature = 100;
  client.onMessageArrived = function (message) {
    var topic = message.destinationName;
    var payload = message.payloadString;
    if (topic === "home/status") {
      showStatus(payload);
    } else if (topic === "home/temperature") {
      temperature = (10.0 + parseFloat(payload) / 51.2);
      setTemperature(temperature);
    }
  };

  var chart = new SmoothieChart({minValue: 10.0, maxValue: 30.0});
  var canvas = document.getElementById("smoothie-chart");
  var series = new TimeSeries();

  chart.addTimeSeries(series,
      { strokeStyle:'rgb(255, 255, 0)', fillStyle:'rgba(0, 255, 255, 0.3)', lineWidth:3 });
  chart.streamTo(canvas, 750);

  setTemperature = function(newTemperature) {
    series.append(new Date().getTime(), newTemperature);
    gauge.refresh(newTemperature.toFixed(1));
    messageCount++;
    $('#txtMessageCount').text(messageCount);
  };

  showStatus = function(statusMessage) {
    $('#txtStatus').text(statusMessage);
  };

  setInterval(function() {
    $('#txtMessagesPerSecond').text(messageCount - messagesPerSecond);
    messagesPerSecond = messageCount;
  }, 1000);

  client.connect(options);
});
