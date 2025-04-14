# Redis w. Retention Policy


























## 🛡️ Configuration 4: Redis Security

### 🔐 Basic Authentication

```bash
1. docker exec -it redis redis-cli         # Access Redis CLI inside Docker container
2. CONFIG SET requirepass test             # Set a password for Redis access
3. SETEX user:1 20 "John Doe"              # Try to set a key with expiry
4. # Expected: (error) NOAUTH Authentication required.
5. AUTH test                               # Authenticate with the password
6. CONFIG SET requirepass "test"           # Reconfirm the password
7. GET user:1                              # Now retrieve the value successfully
8. ACL SETUSER user1 on >test ~* +GET +INFO  # Create user1 with read and info access
9. ACL SETUSER user2 on >test ~* +GET +SET   # Create user2 with read/write access
10. ACL LIST                                 # List all ACL users and permissions
11. AUTH user1 test      # Authenticate as user1
12. GET key              # Allowed: user1 can GET
13. SET key 1            # Expected: (error) permission denied for SET
