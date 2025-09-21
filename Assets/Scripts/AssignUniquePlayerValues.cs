using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputStore))]
public class AssignUniquePlayerValues : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public CinemachineCamera cinemachineCamera1;
    public CinemachineCamera cinemachineCamera2;

    public SkinnedMeshRenderer meshRenderer;
    public Material player2Material;

    private int useSpecificCameraOnStart = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() // Don't change this please or you will ruin everyting
    {
        int playerindex = GetComponent<PlayerInputStore>().playerIndex;
        OutputChannels channelToUse = (OutputChannels)(2 << playerindex);

        if (useSpecificCameraOnStart != -1) playerindex = useSpecificCameraOnStart;
        cinemachineBrain.ChannelMask = channelToUse;
        cinemachineCamera1.gameObject.SetActive(playerindex == 0);
        cinemachineCamera2.gameObject.SetActive(playerindex == 1);
        CinemachineCamera usingCamera = playerindex == 0 ? cinemachineCamera1 : cinemachineCamera2;
        usingCamera.OutputChannel = channelToUse;
        usingCamera.GetComponent<CinemachineInputAxisController>().PlayerIndex = playerindex;

        Debug.Log("Channel = " + channelToUse);

        if (playerindex > 0 || useSpecificCameraOnStart != -1) {
            Destroy(cinemachineBrain.GetComponent<AudioListener>());
            AssignP2Material();
        }
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

    public void SetUseSpeciifCameraOnStart(int cameraNum)
    {
        useSpecificCameraOnStart = cameraNum;
    }


    public void AssignP2Material()
    {
        // meshRenderer.materials[0] = player2Material;
        meshRenderer.material = player2Material;
        Debug.Log("assigning material 2");
    }
}
