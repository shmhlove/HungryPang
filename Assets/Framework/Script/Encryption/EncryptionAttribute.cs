using UnityEngine;
using System.Collections;

public class EncInt : Attribute<int>
{
    public EncInt(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncInt(string name, bool encrypt, int value)
        : base(name, encrypt, value)
    {

    }
}

public class EncUInt : Attribute<uint>
{
    public EncUInt(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncUInt(string name, bool encrypt, uint value)
        : base(name, encrypt, value)
    {

    }
}

public class EncFloat : Attribute<float>
{
    public EncFloat(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncFloat(string name, bool encrypt, float value)
        : base(name, encrypt, value)
    {

    }
}

public class EncBool : Attribute<bool>
{
    public EncBool(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncBool(string name, bool encrypt, bool value)
        : base(name, encrypt, value)
    {

    }
}

public class EncShort : Attribute<short>
{
    public EncShort(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncShort(string name, bool encrypt, short value)
        : base(name, encrypt, value)
    {

    }
}

public class EncUShort : Attribute<ushort>
{
    public EncUShort(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncUShort(string name, bool encrypt, ushort value)
        : base(name, encrypt, value)
    {

    }
}

public class EncChar : Attribute<char>
{
    public EncChar(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncChar(string name, bool encrypt, char value)
        : base(name, encrypt, value)
    {

    }
}

public class EncLong : Attribute<long>
{
    public EncLong(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncLong(string name, bool encrypt, long value)
        : base(name, encrypt, value)
    {

    }
}

public class EncULong : Attribute<ulong>
{
    public EncULong(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncULong(string name, bool encrypt, ulong value)
        : base(name, encrypt, value)
    {

    }
}

public class EncDouble : Attribute<double>
{
    public EncDouble(string name, bool encrypt = false)
        : base(name, encrypt)
    {

    }

    public EncDouble(string name, bool encrypt, double value)
        : base(name, encrypt, value)
    {

    }
}


