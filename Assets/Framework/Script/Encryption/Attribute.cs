using UnityEngine;
using System.Collections;
using System;

public class Observable<T>
{
    public delegate void OnPropagated(T target);

    T target;
    private OnPropagated onPropagated;

    public Observable(T target)
    {
        this.target = target;
    }

    public void Propagate()
    {
        if (onPropagated == null)
            return;
        onPropagated(target);
    }

    public void AddObserver(OnPropagated delegateFunc, bool propagateFirst)
    {
        onPropagated += delegateFunc;
        if (propagateFirst)
            delegateFunc(target);
    }

    public void DelObserver(OnPropagated delegateFunc)
    {
        onPropagated -= delegateFunc;
    }
}

public class Attribute<T> where T : struct
{
    // methods...
    public Attribute(string name, bool encrypt = false)
    {
        this.name = name;
        observer = new Observable<Attribute<T>>(this);
        if (encrypt)
        {
            encryptor = new EncBytes(curValue);
        }
    }

    public Attribute(string name, bool encrypt, T value)
    {
        this.name = name;
        observer = new Observable<Attribute<T>>(this);
        if (encrypt)
        {
            encryptor = new EncBytes(curValue);
        }

        this.SetValue(value);
    }

    public void Update()
    {
        if (encryptor != null)
        {
            encryptor.UpdateEnc();
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    // New Method[blueasa / 2014-01-23]
    public T Value
    {
        get
        {
            if (encryptor != null)
            {
                if (encryptor.CheckValue(curValue))
                    return curValue;
                else
                    throw new Exception("detected memory cheating");
            }
            else
            {
                return curValue;
            }
        }

        set
        {
            if (CurValue.Equals(value))
                return;

            lock (thisLock)
            {
                preValue = curValue;
                curValue = value;

                if (encryptor != null)
                {
                    encryptor.SetValue(curValue);
                }

                Propagate();
            }
        }
    }

    private T CurValue
    {
        get
        {
            if (encryptor != null)
            {
                if (encryptor.CheckValue(curValue))
                    return curValue;
                else
                    throw new Exception("detected memory cheating");
            }
            else
            {
                return curValue;
            }
        }
    }

    //public T CurValue {
    //    get {
    //        if( encryptor != null ) {
    //            if( encryptor.CheckValue( curValue ) )
    //                return curValue;
    //            else
    //                throw new Exception("detected memory cheating");	
    //        }
    //        else {
    //            return curValue;
    //        }
    //    }
    //}

    public T PreValue
    {
        get
        {
            return preValue;
        }
    }

    private void SetValue(T newValue)
    {
        if (CurValue.Equals(newValue))
            return;

        lock (thisLock)
        {
            preValue = curValue;
            curValue = newValue;

            if (encryptor != null)
            {
                encryptor.SetValue(curValue);
            }

            Propagate();
        }
    }

    //public void SetValue( T newValue )
    //{
    //    if( CurValue.Equals( newValue ) )
    //        return;

    //    lock(thisLock) {
    //        preValue = curValue;
    //        curValue = newValue;

    //        if( encryptor != null ) {
    //            encryptor.SetValue( curValue );
    //        }

    //        Propagate();
    //    }
    //}

    // observer...
    public void Propagate()
    {
        observer.Propagate();
    }

    public void AddObserver(Observable<Attribute<T>>.OnPropagated delegateFunc, bool propagateFirst = false)
    {
        observer.AddObserver(delegateFunc, propagateFirst);
    }

    public void DelObserver(Observable<Attribute<T>>.OnPropagated delegateFunc)
    {
        observer.DelObserver(delegateFunc);
    }

    // members...
    private string name;
    private T preValue;
    private T curValue;
    private Observable<Attribute<T>> observer = null;
    private EncBytes encryptor = null;

    //
    private object thisLock = new object();
}

public class EncBytes
{
    public EncBytes(object initValue)
    {
        SetConverter(initValue);
        SetValue(initValue);
    }

    public void SetValue(object newValue)
    {
        lock (thisLock)
        {
            encTick = DateTime.Now.Ticks + reEncInterval;
            curKey = BitConverter.GetBytes(encTick);
            curEncBytes = bytesConverter(newValue);
            Encrypt(curEncBytes, curKey);
        }
    }

    public void UpdateEnc()
    {
        if (encTick > DateTime.Now.Ticks)
            return;

        lock (thisLock)
        {
            //			Debug.Log ( "UpdateEnc() = " + encTick );
            encTick = DateTime.Now.Ticks + reEncInterval;
            Encrypt(curEncBytes, curKey);
            curKey = BitConverter.GetBytes(encTick);
            Encrypt(curEncBytes, curKey);
        }
    }

    public bool CheckValue(object oriValue)
    {

        byte[] oriBytes = bytesConverter(oriValue);
        Encrypt(oriBytes, curKey);

        if (ByteArraysEqual(oriBytes, curEncBytes))
            return true;
        else
            return false;
    }

    //
    private void Encrypt(byte[] encBytes, byte[] key)
    {
        if (encBytes == null || key == null)
            return;
        if (encBytes.Length > key.Length)
            return;

        for (int index = 0; index < encBytes.Length; ++index)
        {
            encBytes[index] ^= key[index];
        }
    }

    public bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2)
            return true;
        if (b1 == null || b2 == null)
            return false;
        if (b1.Length != b2.Length)
            return false;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i])
                return false;
        }
        return true;
    }

    //
    static byte[] ShortToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((short)objValue);
    }
    static byte[] UShortToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((ushort)objValue);
    }
    static byte[] IntToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((int)objValue);
    }
    static byte[] UIntToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((uint)objValue);
    }
    static byte[] LongToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((long)objValue);
    }
    static byte[] ULongToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((ulong)objValue);
    }
    static byte[] FloatToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((float)objValue);
    }
    static byte[] DoubleToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((double)objValue);
    }
    static byte[] CharToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((char)objValue);
    }
    static byte[] BoolToBytes(object objValue)
    {
        return System.BitConverter.GetBytes((bool)objValue);
    }

    private void SetConverter(object obj)
    {
        if (obj is int)
        {
            bytesConverter = IntToBytes;
        }
        else if (obj is float)
        {
            bytesConverter = FloatToBytes;
        }
        else if (obj is bool)
        {
            bytesConverter = BoolToBytes;
        }
        else if (obj is short)
        {
            bytesConverter = ShortToBytes;
        }
        else if (obj is uint)
        {
            bytesConverter = UIntToBytes;
        }
        else if (obj is ushort)
        {
            bytesConverter = UShortToBytes;
        }
        else if (obj is char)
        {
            bytesConverter = CharToBytes;
        }
        else if (obj is long)
        {
            bytesConverter = LongToBytes;
        }
        else if (obj is ulong)
        {
            bytesConverter = ULongToBytes;
        }
        else if (obj is double)
        {
            bytesConverter = DoubleToBytes;
        }
        else
        {
            throw new Exception("EncBytes.SetConverter() : not compatible type.");
        }
    }


    delegate byte[] ObjectToByteArray(object objValue);

    //
    private int reEncInterval = 2000000;
    private long encTick = 0;
    private byte[] curKey;
    private byte[] curEncBytes;
    private ObjectToByteArray bytesConverter;

    //
    protected object thisLock = new object();
}