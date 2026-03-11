using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class UnityAuthService : IAuthService
{
    public bool IsSignedIn => AuthenticationService.Instance.IsSignedIn;
    public string PlayerId => AuthenticationService.Instance.PlayerId;

    public async Task<AuthResult> SignInAnonymouslyAsync(CancellationToken ct)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
                Debug.Log("[UnityAuthService] UnityServices Initialized");
            }

            ct.ThrowIfCancellationRequested();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            
            Debug.Log("[UnityAuthService] Signed In ID: " + AuthenticationService.Instance.PlayerId);
            
            return new AuthResult 
            { 
                isSuccess = true, 
                playerId = AuthenticationService.Instance.PlayerId 
            };
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("[UnityAuthService] Sign In was canceled by user.");
            throw; 
        }
        catch (Exception e)
        {
            Debug.LogError($"[UnityAuthService] Sign In Failed: {e.Message}");
            return new AuthResult 
            { 
                isSuccess = false, 
                playerId = string.Empty 
            };
        }
    }
}