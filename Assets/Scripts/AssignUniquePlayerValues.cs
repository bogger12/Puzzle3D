using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputStore))]
public class AssignUniquePlayerValues : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public CinemachineCamera cinemachineCamera1;
    public CinemachineCamera cinemachineCamera2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int playerindex = GetComponent<PlayerInputStore>().playerIndex;
        OutputChannels channelToUse = (OutputChannels)(2 << playerindex);
        cinemachineBrain.ChannelMask = channelToUse;
        cinemachineCamera1.gameObject.SetActive(playerindex == 0);
        cinemachineCamera2.gameObject.SetActive(playerindex == 1);
        CinemachineCamera usingCamera = playerindex == 0 ? cinemachineCamera1 : cinemachineCamera2;
        usingCamera.OutputChannel = channelToUse;
        usingCamera.GetComponent<CinemachineInputAxisController>().PlayerIndex = playerindex;
        
        Debug.Log("Channel = " + channelToUse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseSpecificCamera(int cameraNum, int outputChannel = -1)
    {
        OutputChannels channelToUse = outputChannel != -1 ? (OutputChannels)outputChannel : (OutputChannels)(2 << cameraNum);
        cinemachineBrain.ChannelMask = channelToUse;

        cinemachineCamera1.gameObject.SetActive(cameraNum == 0);
        cinemachineCamera2.gameObject.SetActive(cameraNum == 1);

        CinemachineCamera usingCamera = cameraNum == 0 ? cinemachineCamera1 : cinemachineCamera2;
        usingCamera.OutputChannel = channelToUse;
        usingCamera.GetComponent<CinemachineInputAxisController>().PlayerIndex = cameraNum;
        Debug.Log("Channel = " + channelToUse);
    }
}
