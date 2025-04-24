SELECT 
    fk.name AS ForeignKey,
    tp.name AS ParentTable,
    ref.name AS ReferencedTable,
    c1.name AS ParentColumn,
    c2.name AS ReferencedColumn
FROM 
    sys.foreign_keys AS fk
INNER JOIN 
    sys.tables AS tp ON fk.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables AS ref ON fk.referenced_object_id = ref.object_id
INNER JOIN 
    sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN 
    sys.columns AS c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
INNER JOIN 
    sys.columns AS c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
ORDER BY 
    ParentTable, ReferencedTable;
