using System;

namespace Sample
{
	// just some object we can test serialization on
	public class Foo : IEquatable<Foo>
	{
		public int A;
		public float B;
		public bool C;

		public Foo()
		{
			Random rand = new Random();
			A = rand.Next();
			B = (float)rand.NextDouble();
			C = rand.Next() % 2 == 0;
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", A, B, C);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (Foo)) return false;
			return Equals((Foo) obj);
		}

		public bool Equals(Foo other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.A == A && other.B == B && other.C.Equals(C);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = A;
				result = (result*397) ^ B.GetHashCode();
				result = (result*397) ^ C.GetHashCode();
				return result;
			}
		}

		public static bool operator ==(Foo left, Foo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Foo left, Foo right)
		{
			return !Equals(left, right);
		}
	}
}