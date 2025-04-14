# Redis w. Retention Policy

1. **Install Redis on two different machines and configure them to run on different ports**
   - Installed Redis using Docker with:
     - Master on port **6379**
     - Slave on port **6380**

2. **Use the Redis CLI or Redis Telnet CLI to set up a Master-Slave replication configuration between the two Redis instances**
   - Used `redis-cli` to connect the slave to the master using the `SLAVEOF` command
   - Verified data replication from master to slave

3. **Use the Redis CLI or Redis Telnet CLI to store user data in the Master Redis instance**
   - Used the `SET` command to store data:
     ```bash
     SET user:1234 "Alice"
     SET user:4321 "Martin"
     ```
4. **Verify that the Slave Redis instance is replicating data from the Master instance**
   - In the slave Redis CLI, checked for the existence of the keys:
     ```bash
     GET user:1234
     GET user:4321
     ```
5. **Test the configuration by stopping the Master Redis instance and verifying that the Slave Redis instance can handle requests**
   - Stopped the master:
     ```bash
     docker stop redis-master
     ```
   - Tried to read keys that were written earlier:
     ```bash
     GET user:1234  # Returns "Alice"
     GET user:4321  # Returns "Martin"
     ```
   - Verified that data was still accessible from the slave, confirming replication worked

   - Attempted to write to the slave:
     ```bash
     SET user:5678 "Bob"
     ```
   - Received an error since the slave is **read-only by default** — writes are not allowed unless it is promoted to master




















## 🛡️ Configuration 4: Redis Security

### 🔐 Basic Authentication

```bash
1. docker exec -it redis redis-cli         # Access Redis CLI inside Docker container
2. CONFIG SET requirepass test             # Set a password for Redis access
3. SETEX user:1 20 "John Doe"              # Try to set a key with expiry
4. # Expected: (error) NOAUTH Authentication required.
5. AUTH test                               # Authenticate with the password
6. CONFIG SET requirepass test           # Reconfirm the password
7. GET user:1                              # Now retrieve the value successfully
8. ACL SETUSER user1 on >test ~* +GET +INFO  # Create user1 with read and info access
9. ACL SETUSER user2 on >test ~* +GET +SET   # Create user2 with read/write access
10. ACL LIST                                 # List all ACL users and permissions
11. AUTH user1 test      # Authenticate as user1
12. GET key              # Allowed: user1 can GET
13. SET key 1            # Expected: (error) permission denied for SET
