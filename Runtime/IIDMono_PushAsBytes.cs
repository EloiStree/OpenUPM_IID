using UnityEngine;


public interface I_IID_IndexGet { 

    public void GetIndex(out int index);
}
public interface I_IID_IntegerGet { 

    public void GetInteger(out int value);

}
public interface I_IID_DateGet { 

    public void GetDate(out ulong date);
}


public interface I_IID_IndexIntegerGet : I_IID_IndexGet, I_IID_IntegerGet { }
public interface I_IID_IntegerDateGet : I_IID_IntegerGet, I_IID_DateGet { }
public interface I_IID_IndexIntegerDateGet : I_IID_IndexGet, I_IID_IntegerGet, I_IID_DateGet { }


public interface I_IID_IndexSet { 

    public void SetIndex(int index);
}

public interface I_IID_IntegerSet { 

    public void SetInteger(int value);
}

public interface I_IID_DateSet { 

    public void SetDate(ulong date);
}


public interface I_IID_IndexIntegerSet : I_IID_IndexSet, I_IID_IntegerSet { }
public interface I_IID_IntegerDateSet : I_IID_IntegerSet, I_IID_DateSet { }
public interface I_IID_IndexIntegerDateSet : I_IID_IndexSet, I_IID_IntegerSet, I_IID_DateSet { }


public interface I_IID_Integer  : I_IID_IntegerGet, I_IID_IntegerSet { }
public interface I_IID_IndexInteger : I_IID_IndexIntegerGet, I_IID_IndexIntegerSet { }
public interface I_IID_IntegerDate : I_IID_IntegerDateGet, I_IID_IntegerDateSet { }
public interface I_IID_IndexIntegerDate : I_IID_IndexIntegerDateGet, I_IID_IndexIntegerDateSet { }



[System.Serializable]
public struct STRUCT_IndexIntegerDate:I_IID_IndexIntegerDate  {
    public int m_index;
    public int m_value;
    public ulong m_date;

    public void GetDate(out ulong date)
    {
        date = m_date;
    }

    public void GetIndex(out int index)
    {
        index = m_index;
    }

    public void GetInteger(out int value)
        {
        value = m_value;
    }

    public void SetDate(ulong date)
        {
        m_date = date;
    }

    public void SetIndex(int index)
        {
        m_index = index;
    }   
    public void SetInteger(int value)
        {
        m_value = value;
    }
}
[System.Serializable]
public struct STRUCT_IntegerDate: I_IID_IntegerDate
{
    public int m_index;
    public ulong m_date;

    public void GetDate(out ulong date) { 
        date = m_date;
    }
    public void GetInteger(out int value)
    {
        value = m_index;

    }

    public void SetDate(ulong date) { 
        m_date = date;
    }

    public void SetInteger(int value) { 
        m_index = value;
    }
}
[System.Serializable]
public struct STRUCT_IndexInteger: I_IID_IndexInteger
{
    public int m_index;
    public int m_value;

    public void GetIndex(out int index)
        { index = m_index;}

    public void GetInteger(out int value)
    { value = m_value;}

    public void SetIndex(int index)
    { m_index = index;}

    public void SetInteger(int value)
    { m_value = value; }
}
[System.Serializable]
public struct STRUCT_Integer : I_IID_Integer
{
    public int m_value;

    public void GetInteger(out int value)
    { value = m_value;}

    public void SetInteger(int value)
        { m_value = value; }
}

//
/// I am a class that received bytes from IID transport and turn them into Unity Events as example
/// <summary>
/// I am a class that push bytes to IID transport from request IID transport
/// </summary>
public class IIDMono_PushAsBytes : MonoBehaviour
{
    

}


public static class IIDUtility { 

   
}