var WebGatewayLib = {
   JsSend: function(type, data) {
        window.WebBridge.send(Pointer_stringify(type), JSON.parse(Pointer_stringify(data)));
   }
}

mergeInto(LibraryManager.library, WebGatewayLib);