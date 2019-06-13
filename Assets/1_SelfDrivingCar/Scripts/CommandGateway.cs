using UnityEngine;
using System;
using UnityStandardAssets.Vehicles.Car;
using System.Security.AccessControl;


[Serializable]
public class CarInputData
{
    public string steering_angle;
    public string throttle;
}

[Serializable]
public class CarOutputData
{
    public string steering_angle;
    public string throttle;
    public string speed;
    public string image;
}

public class CommandGateway : MonoBehaviour
{
	public CarRemoteControl CarRemoteControl;
	public Camera FrontFacingCamera;
	private CarController _carController;

    private WebGateway webGateway;
    // Use this for initialization
    void Start()
	{
        Debug.Log("CommandGateway init");
        webGateway = GameObject.Find("WebGateway").GetComponent<WebGateway>();
        webGateway.MessageReceived += OnMessage;

        _carController = CarRemoteControl.GetComponent<CarController>();
        EmitTelemetry();
       //OnSteer(JsonUtility.FromJson<CarInputData>("{\"steering_angle\": 10, \"throttle\": 10}"));

    }

    void OnMessage(string type, string data)
    {
        switch(type)
        {
            case "open":
                Debug.Log("Connection Open");
                EmitTelemetry();
                break;
            case "steer":
                OnSteer(JsonUtility.FromJson<CarInputData>(data));
                break;
            case "manual":
                EmitTelemetry();
                break;
            default:
                break;
        }
    }


    void OnSteer(CarInputData data)
	{
        CarRemoteControl.SteeringAngle = float.Parse(data.steering_angle);
		CarRemoteControl.Acceleration = float.Parse(data.throttle);
		EmitTelemetry();
	}

	void EmitTelemetry()
	{
		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			//Debug.Log("Attempting to Send...");
			// send only if it's not being manually driven
			if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) {
				webGateway.SendMessage("telemetry", "");
			}
			else {
                // Collect Data from the Car
                CarOutputData carData = new CarOutputData
                {
                    steering_angle = _carController.CurrentSteerAngle.ToString("N4"),
                    throttle = _carController.AccelInput.ToString("N4"),
                    speed = _carController.CurrentSpeed.ToString("N4"),
                    image = Convert.ToBase64String(CameraHelper.CaptureFrame(FrontFacingCamera))
                };

                webGateway.Send("telemetry", JsonUtility.ToJson(carData));
            }
		});

	}
}