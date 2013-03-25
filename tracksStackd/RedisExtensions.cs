using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tracksStackd
{
    public static class RedisExtensions
    {
        public static void UpdateListItem<T>(this IRedisTypedClient<T> redis, string key, T old, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromList(r.Lists[key], old));
                transaction.QueueCommand(r => r.AddItemToList(r.Lists[key], newer));
                transaction.Commit();
            }
        }

        public static void UpdateListItem<T>(this IRedisTypedClient<T> redis, IRedisList<T> list, T old, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromList(list, old));
                transaction.QueueCommand(r => r.AddItemToList(list, newer));
                transaction.Commit();
            }
        }

        public static void UpdateHashItem<T, TKey>(this IRedisTypedClient<T> redis, string hashkey, TKey itemkey, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveEntryFromHash(r.GetHash<TKey>(hashkey), itemkey));
                transaction.QueueCommand(r => r.SetEntryInHash(r.GetHash<TKey>(hashkey), itemkey, newer));
                transaction.Commit();
            }
        }

        public static void UpdateHashItem<T, TKey>(this IRedisTypedClient<T> redis, IRedisHash<TKey, T> hash, TKey itemkey, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveEntryFromHash(hash, itemkey));
                transaction.QueueCommand(r => r.SetEntryInHash(hash, itemkey, newer));
                transaction.Commit();
            }
        }

        public static void UpdateSetItem<T>(this IRedisTypedClient<T> redis, string setkey, T old, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromSet(r.Sets[setkey], old));
                transaction.QueueCommand(r => r.AddItemToSet(r.Sets[setkey], newer));
                transaction.Commit();
            }
        }

        public static void UpdateSetItem<T>(this IRedisTypedClient<T> redis, IRedisSet<T> set, T old, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromSet(set, old));
                transaction.QueueCommand(r => r.AddItemToSet(set, newer));
                transaction.Commit();
            }
        }

        public static void UpdateSortedSetItem<T>(this IRedisTypedClient<T> redis, string setkey, T old, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromSortedSet(r.SortedSets[setkey], old));
                transaction.QueueCommand(r => r.AddItemToSortedSet(r.SortedSets[setkey], newer));
                transaction.Commit();
            }
        }

        public static void UpdateSortedSetItem<T>(this IRedisTypedClient<T> redis, IRedisSortedSet<T> set, T old, T newer)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromSortedSet(set, old));
                transaction.QueueCommand(r => r.AddItemToSortedSet(set, newer));
                transaction.Commit();
            }
        }
        public static void UpdateSortedSetItem<T>(this IRedisTypedClient<T> redis, string setkey, T old, T newer, double score)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromSortedSet(r.SortedSets[setkey], old));
                transaction.QueueCommand(r => r.AddItemToSortedSet(r.SortedSets[setkey], newer, score));
                transaction.Commit();
            }
        }

        public static void UpdateSortedSetItem<T>(this IRedisTypedClient<T> redis, IRedisSortedSet<T> set, T old, T newer, double score)
        {
            using (var transaction = redis.CreateTransaction())
            {
                transaction.QueueCommand(r => r.RemoveItemFromSortedSet(set, old));
                transaction.QueueCommand(r => r.AddItemToSortedSet(set, newer, score));
                transaction.Commit();
            }
        }
    }
}
