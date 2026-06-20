from dlt_typesense.factory import TypesenseTypeMapper, typesense


def test_typesense_type_mapper() -> None:
    """Test type mapping logic - this is pure Python logic, unit test is appropriate."""
    mapper = TypesenseTypeMapper(typesense().capabilities())

    # Check SCT to Typesense mapping
    assert mapper.to_destination_type({"data_type": "text"}, None) == "string"
    assert mapper.to_destination_type({"data_type": "double"}, None) == "float"
    assert mapper.to_destination_type({"data_type": "bool"}, None) == "bool"
    assert mapper.to_destination_type({"data_type": "timestamp"}, None) == "int64"
    assert mapper.to_destination_type({"data_type": "date"}, None) == "int64"
    assert mapper.to_destination_type({"data_type": "time"}, None) == "string"
    assert mapper.to_destination_type({"data_type": "bigint"}, None) == "int64"
    assert mapper.to_destination_type({"data_type": "binary"}, None) == "string"
    assert mapper.to_destination_type({"data_type": "decimal"}, None) == "string"
    assert mapper.to_destination_type({"data_type": "wei"}, None) == "string"
    assert mapper.to_destination_type({"data_type": "json"}, None) == "object"
    assert (
        mapper.to_destination_type(
            {"data_type": "json", "x-typesense-facet": True}, None
        )
        == "string[]"
    )
    assert mapper.to_destination_type({"data_type": "complex"}, None) == "object"

    # Array type mapping should preserve array syntax
    assert mapper.to_destination_type({"data_type": "text[]"}, None) == "string[]"
    assert mapper.to_destination_type({"data_type": "double[]"}, None) == "float[]"
    assert mapper.to_destination_type({"data_type": "bigint[]"}, None) == "int64[]"

    # Check Typesense to SCT mapping
    assert mapper.from_destination_type("string", None, None) == {"data_type": "text"}
    assert mapper.from_destination_type("float", None, None) == {"data_type": "double"}
    assert mapper.from_destination_type("bool", None, None) == {"data_type": "bool"}
    assert mapper.from_destination_type("int64", None, None) == {"data_type": "bigint"}
    assert mapper.from_destination_type("int32", None, None) == {"data_type": "bigint"}
