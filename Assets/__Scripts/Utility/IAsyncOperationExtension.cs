using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class IAsyncOperationExtension
{
    //with a little bit of extension and sugar
    async public static UniTask<T> ToUniTask<T>( this IAsyncOperation<T> operation )
    {
        await operation;
        return operation.Result;
    }
}
