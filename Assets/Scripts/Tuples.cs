using System;
using System.Collections.Generic;

/// <summary>
///     Implements a tuple with two values.
/// </summary>
public sealed class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>>, ICloneable
{
    private T1 _t1;
    private T2 _t2;
    public Tuple(T1 t1, T2 t2)
    {
        _t1 = t1;
        _t2 = t2;
    }
    public object this[int ix]
    {
        get
        {
            if (ix == 0)
                return _t1;
            else if (ix == 1)
                return _t2;
            else throw new IndexOutOfRangeException();
        }
        set
        {
            if (ix == 0)
            {
                if (value is T1)
                    _t1 = (T1)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T1)));
            }
            else if (ix == 1)
            {
                if (value is T2)
                    _t2 = (T2)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T2)));
            }
            else throw new IndexOutOfRangeException();
        }
    }
    public override string ToString()
    {
        return String.Format("({0}, {1})", _t1, _t2);
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Tuple<T1, T2>))
            throw new InvalidCastException();
        var objTup = obj as Tuple<T1, T2>;
        return _t1.Equals(objTup[0]) && _t2.Equals(objTup[1]);
    }
    public static bool operator ==(Tuple<T1, T2> dis, Tuple<T1, T2> dat)
    { return dis[0].Equals(dat[0]) && dis[1].Equals(dat[1]); }
    public static bool operator !=(Tuple<T1, T2> dis, Tuple<T1, T2> dat)
    { return !dis[0].Equals(dat[0]) && !dis[1].Equals(dat[1]); }
    public override int GetHashCode()
    {
        int hashCode = 1274918310;
        hashCode = hashCode * -1521134295 + EqualityComparer<T1>.Default.GetHashCode(_t1);
        hashCode = hashCode * -1521134295 + EqualityComparer<T2>.Default.GetHashCode(_t2);
        return hashCode;
    }
    public bool Equals(Tuple<T1, T2> other)
    {
        return _t1.Equals(other[0]) && _t2.Equals(other[1]);
    }
    public object Clone()
    {
        return new Tuple<T1, T2>(_t1, _t2);
    }
    public IEnumerable<object> AsEnumerable()
    {
        yield return _t1;
        yield return _t2;
    }
}

/// <summary>
///     Implements a tuple with three values.
/// </summary>
public sealed class Tuple<T1, T2, T3> : IEquatable<Tuple<T1, T2, T3>>, ICloneable
{
    private T1 _t1;
    private T2 _t2;
    private T3 _t3;
    public Tuple(T1 t1, T2 t2, T3 t3)
    {
        _t1 = t1;
        _t2 = t2;
        _t3 = t3;
    }
    public object this[int ix]
    {
        get
        {
            if (ix == 0)
                return _t1;
            else if (ix == 1)
                return _t2;
            else if (ix == 2)
                return _t3;
            else throw new IndexOutOfRangeException();
        }
        set
        {
            if (ix == 0)
            {
                if (value is T1)
                    _t1 = (T1)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T1)));
            }
            else if (ix == 1)
            {
                if (value is T2)
                    _t2 = (T2)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T2)));
            }
            else if (ix == 2)
            {
                if (value is T3)
                    _t3 = (T3)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T3)));
            }
            else throw new IndexOutOfRangeException();
        }
    }
    public override string ToString()
    {
        return String.Format("({0}, {1}, {2})", _t1, _t2, _t3);
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Tuple<T1, T2, T3>))
            throw new InvalidCastException();
        var objTup = obj as Tuple<T1, T2, T3>;
        return _t1.Equals(objTup[0]) && _t2.Equals(objTup[1]) && _t3.Equals(objTup[2]);
    }
    public static bool operator ==(Tuple<T1, T2, T3> dis, Tuple<T1, T2, T3> dat)
    { return dis[0].Equals(dat[0]) && dis[1].Equals(dat[1]) && dis[2].Equals(dat[2]); }
    public static bool operator !=(Tuple<T1, T2, T3> dis, Tuple<T1, T2, T3> dat)
    { return !dis[0].Equals(dat[0]) && !dis[1].Equals(dat[1]) && !dis[2].Equals(dat[2]); }
    public override int GetHashCode()
    {
        int hashCode = 1274918310;
        hashCode = hashCode * -1521134295 + EqualityComparer<T1>.Default.GetHashCode(_t1);
        hashCode = hashCode * -1521134295 + EqualityComparer<T2>.Default.GetHashCode(_t2);
        hashCode = hashCode * -1521134295 + EqualityComparer<T3>.Default.GetHashCode(_t3);
        return hashCode;
    }
    public bool Equals(Tuple<T1, T2, T3> other)
    {
        return _t1.Equals(other[0]) && _t2.Equals(other[1]) && _t3.Equals(other[2]);
    }
    public object Clone()
    {
        return new Tuple<T1, T2, T3>(_t1, _t2, _t3);
    }
    public IEnumerable<object> AsEnumerable()
    {
        yield return _t1;
        yield return _t2;
        yield return _t3;
    }
}

/// <summary>
///     Implements a tuple with four values.
/// </summary>
public sealed class Tuple<T1, T2, T3, T4> : IEquatable<Tuple<T1, T2, T3, T4>>, ICloneable
{
    private T1 _t1;
    private T2 _t2;
    private T3 _t3;
    private T4 _t4;
    public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
    {
        _t1 = t1;
        _t2 = t2;
        _t3 = t3;
        _t4 = t4;
    }
    public object this[int ix]
    {
        get
        {
            if (ix == 0)
                return _t1;
            else if (ix == 1)
                return _t2;
            else if (ix == 2)
                return _t3;
            else if (ix == 3)
                return _t4;
            else throw new IndexOutOfRangeException();
        }
        set
        {
            if (ix == 0)
            {
                if (value is T1)
                    _t1 = (T1)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T1)));
            }
            else if (ix == 1)
            {
                if (value is T2)
                    _t2 = (T2)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T2)));
            }
            else if (ix == 2)
            {
                if (value is T3)
                    _t3 = (T3)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T3)));
            }
            else if (ix == 3)
            {
                if (value is T4)
                    _t4 = (T4)value;
                else throw new InvalidCastException(String.Format("Cannot cast to " + typeof(T4)));
            }
            else throw new IndexOutOfRangeException();
        }
    }
    public override string ToString()
    {
        return String.Format("({0}, {1}, {2}, {3})", _t1, _t2, _t3, _t4);
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Tuple<T1, T2, T3, T4>))
            throw new InvalidCastException();
        var objTup = obj as Tuple<T1, T2, T3, T4>;
        return _t1.Equals(objTup[0]) && _t2.Equals(objTup[1]) && _t3.Equals(objTup[2]) && _t4.Equals(objTup[3]);
    }
    public static bool operator ==(Tuple<T1, T2, T3, T4> dis, Tuple<T1, T2, T3, T4> dat)
    { return dis[0].Equals(dat[0]) && dis[1].Equals(dat[1]) && dis[2].Equals(dat[2]) && dis[3].Equals(dat[3]); }
    public static bool operator !=(Tuple<T1, T2, T3, T4> dis, Tuple<T1, T2, T3, T4> dat)
    { return !dis[0].Equals(dat[0]) && !dis[1].Equals(dat[1]) && !dis[2].Equals(dat[2]) && !dis[3].Equals(dat[3]); }
    public override int GetHashCode()
    {
        int hashCode = 1274918310;
        hashCode = hashCode * -1521134295 + EqualityComparer<T1>.Default.GetHashCode(_t1);
        hashCode = hashCode * -1521134295 + EqualityComparer<T2>.Default.GetHashCode(_t2);
        hashCode = hashCode * -1521134295 + EqualityComparer<T3>.Default.GetHashCode(_t3);
        hashCode = hashCode * -1521134295 + EqualityComparer<T4>.Default.GetHashCode(_t4);
        return hashCode;
    }
    public bool Equals(Tuple<T1, T2, T3, T4> other)
    {
        return _t1.Equals(other[0]) && _t2.Equals(other[1]) && _t3.Equals(other[2]) && _t4.Equals(other[3]);
    }
    public object Clone()
    {
        return new Tuple<T1, T2, T3, T4>(_t1, _t2, _t3, _t4);
    }
    public IEnumerable<object> AsEnumerable()
    {
        yield return _t1;
        yield return _t2;
        yield return _t3;
        yield return _t4;
    }
}