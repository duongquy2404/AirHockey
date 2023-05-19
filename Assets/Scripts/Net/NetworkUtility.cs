using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public class NetworkUtility : MonoBehaviour
{
    private static bool isHeadlessResult;
    private static bool isHeadlessCache = false;
    
    public static bool IsBatchModeRun
    {
        get
        {
#if ENABLE_AUTO_CLIENT
                if (isHeadlessCache)
                {
                    return isHeadlessResult;
                }
                isHeadlessResult = IsBatchMode();
                isHeadlessCache = true;
#else
            isHeadlessResult = false;
#endif
            return isHeadlessResult;
        }
    }
    
    public static string GetLocalIP()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        IPHostEntry ipentry = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in ipentry.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                sb.Append(ip.ToString());
            }
        }

        return sb.ToString();
    }


    // Batch Mode起動かどうか調べて返します
    private static bool IsBatchMode()
    {
        var commands = System.Environment.GetCommandLineArgs();

        foreach (var command in commands)
        {
            if (command.ToLower().Trim() == "-batchmode")
            {
                return true;
            }
        }

        return (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    
    public static void RemoveUpdateSystemForHeadlessServer()
    {
        RemoveUpdateSystem(ShouldExcludeForHeadless, true);
    }
    
    public static void RemoveUpdateSystemForBatchBuild()
    {
        RemoveUpdateSystem(ShouldExcludeForHeadless, false);
    }
    
    private static void RemoveUpdateSystem(System.Func<PlayerLoopSystem, bool> shouldExcludeFunc, bool removeAllPhysics)
    {
        var currentLoop = PlayerLoop.GetCurrentPlayerLoop();
        var replaceSubSystems = new List<PlayerLoopSystem>();
        var replaceUpdateSystems = new List<PlayerLoopSystem>();

        foreach (var subsystem in currentLoop.subSystemList)
        {
            if (removeAllPhysics &&
                subsystem.type == typeof(UnityEngine.PlayerLoop.FixedUpdate))
            {
                continue;
            }

            replaceUpdateSystems.Clear();
            var newSubSystem = subsystem;

            foreach (var updateSystem in subsystem.subSystemList)
            {
                if (!shouldExcludeFunc(updateSystem))
                {
                    replaceUpdateSystems.Add(updateSystem);
                }
            }

            newSubSystem.subSystemList = replaceUpdateSystems.ToArray();
            replaceSubSystems.Add(newSubSystem);
        }

        currentLoop.subSystemList = replaceSubSystems.ToArray();

        PlayerLoop.SetPlayerLoop(currentLoop);
    }
    
    private static bool ShouldExcludeForHeadless(PlayerLoopSystem updateSystem)
    {
        return

            
            (updateSystem.type == typeof(PreUpdate.SendMouseEvents)) ||

            
            (updateSystem.type == typeof(PreUpdate.NewInputUpdate)) ||

            
            (updateSystem.type == typeof(PostLateUpdate.UpdateAudio)) ||

            
            (updateSystem.type == typeof(PreLateUpdate.DirectorUpdateAnimationBegin)) ||
            (updateSystem.type == typeof(PreLateUpdate.DirectorDeferredEvaluate)) ||
            (updateSystem.type == typeof(PreLateUpdate.DirectorUpdateAnimationEnd)) ||
            (updateSystem.type == typeof(Update.DirectorUpdate)) ||
            (updateSystem.type == typeof(PreLateUpdate.LegacyAnimationUpdate)) ||
            (updateSystem.type == typeof(PreLateUpdate.ConstraintManagerUpdate)) ||

            
            (updateSystem.type == typeof(PreLateUpdate.ParticleSystemBeginUpdateAll)) ||
            (updateSystem.type == typeof(PostLateUpdate.ParticleSystemEndUpdateAll)) ||

            
            (updateSystem.type == typeof(PostLateUpdate.UpdateVideoTextures)) ||
            (updateSystem.type == typeof(PostLateUpdate.UpdateVideo)) ||

            
            (updateSystem.type == typeof(PostLateUpdate.UpdateAllRenderers)) ||
            (updateSystem.type == typeof(PostLateUpdate.UpdateAllSkinnedMeshes)) ||


            (updateSystem.type == typeof(PostLateUpdate.PlayerUpdateCanvases)) ||
            (updateSystem.type == typeof(PostLateUpdate.PlayerEmitCanvasGeometry)) ||
            (updateSystem.type == typeof(PreUpdate.AIUpdate)) ||
            (updateSystem.type == typeof(PreLateUpdate.AIUpdatePostScript)) ||
            false;
    }

    
    public class RequireAtHeadless : System.Attribute
    {
    }
    
    public static void RemoveAllStandaloneComponents(GameObject gmo)
    {
        var allComponents = gmo.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var component in allComponents)
        {
            if (component is Unity.Netcode.NetworkObject ||
                component is Unity.Netcode.NetworkBehaviour)
            {
                continue;
            }

            var attr = component.GetType().GetCustomAttribute(typeof(RequireAtHeadless), false);
            if (attr != null)
            {
                continue;
            }

            Object.Destroy(component);
        }
    }
}
