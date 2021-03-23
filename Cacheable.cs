using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public abstract class Cacheable<K, V>
{
    private Dictionary<K, V> cache;
    private K id;

    public Cacheable(K id, Dictionary<K, V> cache) {
        this.cache = cache;
        this.id = id;
        Kill();
    }

    private async void Kill() {
        await Task.Delay(600000);
        cache.Remove(id);
    }
}
