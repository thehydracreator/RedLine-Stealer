public static class ObjectEx
{
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T val = lhs;
        lhs = rhs;
        rhs = val;
    }
}
