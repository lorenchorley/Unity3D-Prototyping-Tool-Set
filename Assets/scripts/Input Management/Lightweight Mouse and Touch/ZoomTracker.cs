
public class ZoomTracker {

    private float speed = 1;
    private float min = 0;
    private float max = 1;
    private float current = 0;

    public ZoomTracker() {

    }

    public ZoomTracker(float min, float max) {
        this.min = min;
        this.max = max;
    }

    public ZoomTracker(float min, float max, float current) {
        this.min = min;
        this.max = max;
        this.current = current;
    }

    public float GetZoomLevel() {
        return current;
    }

    public void AdjustBy(float adjustment) {
        current += adjustment * speed;
        ApplyBounds();
    }

    private void ApplyBounds() {
        if (current > max) {
            current = max;
        } else if (current < min) {
            current = min;
        }
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetMin(float min) {
        this.min = min;
        ApplyBounds();
    }

    public void SetMax(float max) {
        this.max = max;
        ApplyBounds();
    }

}

