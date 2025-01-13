[System.Serializable]
public struct STRUCT_IndexInteger: I_IID_IndexIntegerGetSet
{
    public int m_index;
    public int m_value;

  
    public int GetIndex()
    { return m_index; }

    public int GetInteger()
    { return m_value; }


    public void GetIndex(out int index)
        { index = m_index;}

    public void GetInteger(out int value)
    { value = m_value;}

    public void SetIndex(int index)
    { m_index = index;}

    public void SetInteger(int value)
    { m_value = value; }
}

