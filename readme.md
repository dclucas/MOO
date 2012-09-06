Moo: an object-to-object multimapper
====================================

Moo allows you to compose multiple mapping strategies into one for your object-to-object mapping.

Mapping strategies
------------------

Out-of-box, the following strategies are available, but you can also create your own:

1. Convention: matches by name and type, with property unfolding;
2. Attributes: mark your classes (source and/or target) with mapping attributes;
3. Configuration: add a mapping section to your .config file and this mapper will follow it;
4. Manual: explicit code calls do the mapping;

Usage
-----

## Simple usage

Mapping can ba as simple as this:

    var source = CreateSource();
    var result = source.MapTo<PersonEditModel>();

This extension method does all the work under the hood, creating a mapper, a repository, etc.

