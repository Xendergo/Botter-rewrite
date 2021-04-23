CREATE TABLE items (
  itemId bigserial NOT NULL,
  id TEXT NOT NULL,
  name TEXT NOT NULL,
  data JSON NOT NULL
)
-- @block
SELECT * FROM items
-- @block
UPDATE items SET name = 'golden-sword' WHERE itemid = 1