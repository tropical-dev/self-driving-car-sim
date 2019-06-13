import io from 'socket.io-client';

const socket = io('http://localhost:4567');

socket.on('connect', function(){
    console.log('connected')
});
socket.on('event', function(data){
    console.log(data)
});
socket.on('disconnect', function(){});

socket.on('steer', function(data) {
    sendMessage(data);
});

var WebBridge = {
    send: function(type, data) {
       socket.emit("telemetry", data)
    }
  }

var unityInstance = UnityLoader.instantiate("unityContainer", "player/Build/player.json", {onProgress: UnityProgress});

function sendMessage(str_data) {
    //let data = JSON.stringify({"steering_angle": "-0.01", "throttle": "10"});
    let data = JSON.stringify(str_data);
    let msg = JSON.stringify({type: "steer", data: data});
    unityInstance.SendMessage("WebGateway", "Receive", msg);
}

window.unityInstance = unityInstance;
window.WebBridge = WebBridge;


//  unityInstance.SendMessage("WebGateway", "Receive", JSON.stringify({type: "steer", data: JSON.stringify({"steering_angle": "-0.01", "throttle": "10"})}))