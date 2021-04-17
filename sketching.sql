-- @block
CREATE TABLE users (
  id text,
  GotSniped integer DEFAULT 0,
  PeopleSniped integer DEFAULT 0,
  PeopleKilled integer DEFAULT 0,
  SelfSniped integer DEFAULT 0,
  Died integer DEFAULT 0,
  Searched integer DEFAULT 0,
  Interactions integer DEFAULT 0
)

-- @block
ALTER TABLE users
ADD health integer DEFAULT 100

-- @block
SELECT * FROM users