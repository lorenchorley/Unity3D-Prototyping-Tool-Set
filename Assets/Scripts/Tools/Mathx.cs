
public static class Mathx {
        
    // A modulo function that works as expected for negative numbers
    public static int ModN(int x, int n) {
        return (x % n + n) % n;
    }

}
