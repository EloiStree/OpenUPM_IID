[System.Serializable]
public struct STRUCT_Integer : I_IID_IntegerGetSet
{
    public int m_value;

    public void GetInteger(out int value)
    { value = m_value;}

    public int GetInteger()
    { return m_value;}

    public void SetInteger(int value)
        { m_value = value; }
}

