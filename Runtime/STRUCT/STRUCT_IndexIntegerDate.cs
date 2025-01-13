[System.Serializable]
public struct STRUCT_IndexIntegerDate:I_IID_IndexIntegerDateGetSet  {
    public int m_index;
    public int m_value;
    public ulong m_date;

    public void GetDate(out ulong date)
    {
        date = m_date;
    }
    public ulong GetDate()
    {
        return m_date;
    }

    public void GetIndex(out int index)
    {
        index = m_index;
    }
    public int GetIndex()
    {
        return m_index;
    }

    public void GetInteger(out int value)
        {
        value = m_value;
    }
    public int GetInteger()
    {
        return m_value;
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

