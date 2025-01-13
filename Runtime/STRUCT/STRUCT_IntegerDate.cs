[System.Serializable]
public struct STRUCT_IntegerDate: I_IID_IntegerDateGetSet
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
    public ulong GetDate() { 
        return m_date;
    }

    public int GetInteger() { 
        return m_index;
    }
    public void SetDate(ulong date) { 
        m_date = date;
    }

    public void SetInteger(int value) { 
        m_index = value;
    }
}

