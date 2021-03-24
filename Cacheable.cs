using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public abstract class Cacheable<K, V>
{
    private Dictionary<K, V> cache;
    private K id;
    private long timeToKill;

    public Cacheable(K id, Dictionary<K, V> cache) {
        this.cache = cache;
        this.id = id;
        timeToKill = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 600000;
        Kill();
    }

    private async void Kill() {
        long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        while (now < timeToKill) {
            await Task.Delay((int)(timeToKill - now));
        }
        cache.Remove(id);
    }
    public void resetKill() {
        timeToKill = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 600000;
    }
}
