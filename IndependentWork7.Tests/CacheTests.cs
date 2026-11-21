using IndependentWork7;
using Xunit;

namespace IndependentWork7.Tests;

public class CacheTests
{

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_NonPositiveCapacity_Throws(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, string>(capacity));
    }

    [Fact]
    public void NewCache_IsEmpty()
    {
        var cache = new Cache<int, string>(3);

        Assert.Equal(0, cache.Count);
    }


    [Fact]
    public void Add_SingleItem_IncreasesCount()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "one");

        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void Add_MultipleItems_AllContained()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "one");
        cache.Add(2, "two");
        cache.Add(3, "three");

        Assert.Equal(3, cache.Count);
        Assert.True(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValueWithoutChangingCount()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "one");

        cache.Add(1, "ONE");

        Assert.Equal(1, cache.Count);
        Assert.Equal("ONE", cache.Get(1));
    }


    [Fact]
    public void Get_ExistingKey_ReturnsValue()
    {
        var cache = new Cache<string, int>(2);
        cache.Add("x", 42);

        Assert.Equal(42, cache.Get("x"));
    }

    [Fact]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<int, string>(2);

        Assert.Throws<KeyNotFoundException>(() => cache.Get(99));
    }


    [Fact]
    public void Add_WhenFull_RemovesOldestItem()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");

        cache.Add(3, "three");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
    }

    [Fact]
    public void Add_WhenFull_CountDoesNotExceedCapacity()
    {
        var cache = new Cache<int, int>(3);

        for (int i = 0; i < 10; i++)
            cache.Add(i, i * 10);

        Assert.Equal(3, cache.Count);
    }

    [Fact]
    public void Get_EvictedKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<int, string>(1);
        cache.Add(1, "one");
        cache.Add(2, "two");

        Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        Assert.Equal("two", cache.Get(2));
    }

    [Fact]
    public void Eviction_FollowsInsertionOrder()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        cache.Add(4, "d");
        cache.Add(5, "e"); 

        Assert.False(cache.ContainsKey(1));
        Assert.False(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
        Assert.True(cache.ContainsKey(5));
    }

    [Fact]
    public void Get_DoesNotAffectEvictionOrder_FifoNotLru()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");

        cache.Get(1); 
        cache.Add(3, "three");

        Assert.False(cache.ContainsKey(1)); 
        Assert.True(cache.ContainsKey(2));
    }

    [Fact]
    public void Add_ExistingKeyWhenFull_DoesNotEvictAnything()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");

        cache.Add(1, "ONE");

        Assert.Equal(2, cache.Count);
        Assert.True(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
    }

    // ---------- ContainsKey ----------

    [Fact]
    public void ContainsKey_MissingKey_ReturnsFalse()
    {
        var cache = new Cache<string, string>(2);

        Assert.False(cache.ContainsKey("nope"));
    }

    // ---------- Count ----------

    [Fact]
    public void Count_TracksAddedItems()
    {
        var cache = new Cache<int, int>(5);

        Assert.Equal(0, cache.Count);
        cache.Add(1, 1);
        Assert.Equal(1, cache.Count);
        cache.Add(2, 2);
        Assert.Equal(2, cache.Count);
    }
}
