# unity-client
Unity Client for TNice application

## Dependencies

Every dependencies listed here should be ever downloaded from the Unity Asset Store or from their git source repository, and be placed into `Assets/AssetStoreTools` directory.

* Socket.IO for Unity: https://assetstore.unity.com/packages/tools/network/socket-io-for-unity-21721
* Oculus Integration: https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022
    * if help is needed, you can follow the tutorial here: https://developer.oculus.com/downloads/package/unity-integration/
    * To solve the "Policy" Error CS0234, comment out the `using System.Security.Policy;` line in `OvrAvatarSkinnedMeshRenderPBSV2Component.cs`
