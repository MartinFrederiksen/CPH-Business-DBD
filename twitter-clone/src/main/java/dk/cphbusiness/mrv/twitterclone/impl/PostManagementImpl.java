package dk.cphbusiness.mrv.twitterclone.impl;

import dk.cphbusiness.mrv.twitterclone.contract.PostManagement;
import dk.cphbusiness.mrv.twitterclone.dto.Post;
import dk.cphbusiness.mrv.twitterclone.util.Time;
import redis.clients.jedis.Jedis;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

public class PostManagementImpl implements PostManagement {
    private Jedis jedis;
    private Time time;

    public PostManagementImpl(Jedis jedis, Time time) {
        this.jedis = jedis;
        this.time = time;
    }

    @Override
    public boolean createPost(String username, String message) {
        if(!jedis.hexists("user#" + username, "username")) return false;
        jedis.hset("post#" + username, ""+ time.getCurrentTimeMillis(), message);
        return true;
    }

    @Override
    public List<Post> getPosts(String username) {
        Map<String, String> posts = jedis.hgetAll("post#" + username);
        List<Post> lPosts = new ArrayList<>();
        for(String ts : posts.keySet()) {
            lPosts.add(new Post(Long.parseLong(ts), posts.get(ts)));
        }
        return lPosts;
        //return jedis.hgetAll("posts#" + username).keySet().stream().map(s -> new Post(Long.parseLong(s), fields.get(s))).toList();
    }

    @Override
    public List<Post> getPostsBetween(String username, long timeFrom, long timeTo) {
        Map<String, String> posts = jedis.hgetAll("post#" + username);
        List<Post> lPosts = new ArrayList<>();
        for(String ts : posts.keySet()) {
            if(Long.parseLong(ts) <= timeTo && Long.parseLong(ts) >= timeFrom) 
            lPosts.add(new Post(Long.parseLong(ts), posts.get(ts)));
        }
        return lPosts;
        //return jedis.hgetAll("posts#" + username).keySet().stream().filter(Long.parseLong(ts) <= timeTo && Long.parseLong(ts) >= timeFrom).map(s -> new Post(Long.parseLong(s), fields.get(s))).toList();
    }
}
