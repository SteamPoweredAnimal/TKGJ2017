namespace Util {
	public static class IntExtension {

		public static bool Within(this int x, int min, int max) {
			return x > min && x < max;
		}

		public static bool WithinInclusive(this int x, int min, int max) {
			return x >= min && x <= max;
		}
	}
}
