using System;
using System.Collections.Generic;
using System.IO;
using UniRx;

// A standardised set of methods to do all the things
public static class PersistenceTools { // TODO Test

    #region Save To File
    public static void SaveToDeviceData(this string path, byte[] binary) {
        File.WriteAllBytes(path, binary);
        throw new NotImplementedException();
    }

    public static void SaveToDeviceData(this byte[] binary, string pathToFile) {
        File.WriteAllBytes(pathToFile, binary);
        throw new NotImplementedException();
    }

    public static void SaveToDeviceData(this object obj, string path) {
        SaveToFile(Serialisation.ToBinary(obj), path);
        throw new NotImplementedException();
    }

    public static void SaveUsingAsPath(this string path, byte[] binary) {
        File.WriteAllBytes(path, binary);
    }

    public static void SaveToFile(this byte[] binary, string pathToFile) {
        File.WriteAllBytes(pathToFile, binary);
    }

    public static void SaveToFile(this object obj, string path) {
        SaveToFile(Serialisation.ToBinary(obj), path);
    }
    #endregion

    #region Load From File
    public static byte[] LoadFromDeviceData(this string path) {
        throw new NotImplementedException();
        //return File.ReadAllBytes(path);
    } 

    public static T LoadFromDeviceData<T>(this string path) {
        throw new NotImplementedException();
        //return Serialisation.To<T>(path.LoadUsingAsPath());
    }

    public static byte[] LoadUsingAsPath(this string path) {
        return File.ReadAllBytes(path);
    }

    public static T LoadUsingAsPath<T>(this string path) {
        return Serialisation.To<T>(path.LoadUsingAsPath());
    }
    #endregion

    #region Upload
    public static UniRx.IObservable<string> SerialiseAndUpload(this object obj, string url, Action<string> completeCallback, Dictionary<string, string> headers = null) {
        return Serialisation.ToBinary(obj).SerialiseAndUpload(url, completeCallback, null, headers);
    }

    public static UniRx.IObservable<string> UploadUsingAsURL(this string url, byte[] binary, Action<string> completeCallback, Dictionary<string, string> headers = null) {
        return binary.Upload(url, completeCallback, null, headers);
    }

    public static UniRx.IObservable<string> SerialiseAndUploadUsingAsURL(this string url, object obj, Action<string> completeCallback, Dictionary<string, string> headers = null) {
        return Serialisation.ToBinary(obj).Upload(url, completeCallback, null, headers);
    }

    public static UniRx.IObservable<string> SerialiseAndUpload(this object obj, string url, Action<string> completeCallback, UniRx.IProgress<float> progressCallback, Dictionary<string, string> headers = null) {
        return Serialisation.ToBinary(obj).Upload(url, completeCallback, progressCallback, headers);
    }

    public static UniRx.IObservable<string> UploadUsingAsURL(this string url, byte[] binary, Action<string> completeCallback, UniRx.IProgress<float> progressCallback, Dictionary<string, string> headers = null) {
        return binary.Upload(url, completeCallback, progressCallback, headers);
    }

    public static UniRx.IObservable<string> SerialiseAndUploadUsingAsURL(this string url, object obj, Action<string> completeCallback, UniRx.IProgress<float> progressCallback, Dictionary<string, string> headers = null) {
        return Serialisation.ToBinary(obj).Upload(url, completeCallback, progressCallback, headers);
    }

    public static UniRx.IObservable<string> Upload(this byte[] binary, string url, Action<string> completeCallback, UniRx.IProgress<float> progressCallback, Dictionary<string, string> headers = null) {
        UniRx.IObservable<string> obs = ObservableWWW.Post(url, binary, headers, progressCallback);
        obs.Subscribe(completeCallback);
        return obs;
    }
    #endregion

    #region Download
    public static UniRx.IObservable<byte[]> PostAndRetrieve(this byte[] postData, string url, Action<byte[]> completeCallback, Dictionary<string, string> headers = null) {
        return url.PostAndRetrieveUsingAsURL(postData, completeCallback, null, headers);
    }

    public static UniRx.IObservable<byte[]> PostAndRetrieve(this byte[] postData, string url, Action<byte[]> completeCallback, UniRx.IProgress<float> progressCallback, Dictionary<string, string> headers = null) {
        return url.PostAndRetrieveUsingAsURL(postData, completeCallback, progressCallback, headers);
    }

    public static UniRx.IObservable<byte[]> PostAndRetrieveUsingAsURL(this string url, byte[] postData, Action<byte[]> completeCallback, Dictionary<string, string> headers = null) {
        return url.PostAndRetrieveUsingAsURL(postData, completeCallback, null, headers);
    }

    public static UniRx.IObservable<byte[]> PostAndRetrieveUsingAsURL(this string url, byte[] postData, Action<byte[]> completeCallback, UniRx.IProgress<float> progressCallback, Dictionary<string, string> headers = null) {
        UniRx.IObservable<byte[]> obs = ObservableWWW.PostAndGetBytes(url, postData, headers, progressCallback);
        obs.Subscribe(completeCallback);
        return obs;
    }

    public static UniRx.IObservable<byte[]> DownloadUsingAsURL(this string url, Action<byte[]> completeCallback, Dictionary<string, string> headers = null) {
        return url.DownloadUsingAsURL(completeCallback, null, headers);
    }

    public static UniRx.IObservable<byte[]> DownloadUsingAsURL(this string url, Action<byte[]> completeCallback, UniRx.IProgress<float> progressCallback, Dictionary<string, string> headers = null) {
        UniRx.IObservable<byte[]> obs = ObservableWWW.GetAndGetBytes(url, headers, progressCallback);
        obs.Subscribe(completeCallback);
        return obs;
    }
    #endregion

}