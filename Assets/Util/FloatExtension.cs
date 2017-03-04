namespace Util {
	public static class FloatExtension {

		public static bool Within(this float x, float min, float max) {
			return x > min && x < max;
		}

		public static bool WithinInclusive(this float x, float min, float max) {
			return x >= min && x <= max;
		}
	}
}
