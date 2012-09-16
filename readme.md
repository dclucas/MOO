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

Download
--------

The recommended approach is to download the binaries through Nuget (version 0.8.0) is available now:

To install Moo through Nuget, run the following command in the Package Manager Console

	PM> Install-Package Moo

You can also use the nightly builds output, available at TeamCity:
http://teamcity.codebetter.com/viewLog.html?buildId=lastSuccessful&buildTypeId=bt641&tab=artifacts

Usage
-----

### Simple usage

Mapping can be as simple as this:

    var result = source.MapTo<PersonEditModel>();

This extension method does all the work under the hood, creating a mapper, a repository, etc.

### Mapping collections

Mapping enumerables can be done this way:

    var result = source.MapAll<Person, PersonEditModel>();

Once again, using the extension method leaves Moo in charge of creating all required inner objects.

### Fluent API

The code below shows how a mapper can be extended with additional mapping actions:

    MappingRepository.Default
       .AddMapping<Person, PersonEditModel>()
       .From(p => p.FirstName + p.LastName)
       .To(pe => pe.Name);

    var result = source.MapTo<PersonEditModel>();

A fluent API is provided for explicit code mappings. Advantages over "pure" by-hand mapping is a) a consistent approach to mapping and handling errors and b) the ability to still combine explicit mappings with other strategies.

Instructions like the ones above are carried by the ManualMapper class, which in term can be configured to have bigger or smaller precedence in comparison to other mappers.

### Using auxiliary mappers

You can use additional mappers for internal properties. The code below is instructing Moo to map property Person.Account to PersonDetailsDataContract.PersonAccount through a mapper. 

    MappingRepository.Default
        .AddMapping<Person, PersonDetailsDataContract>()
        .UseMapperFrom(p => p.Account)
        .To(pd => pd.Account)
        .From(p => p.FirstName + p.LastName)
        .To(pd => pd.Name);

    var result = source.MapTo<PersonDetailsDataContract>();

### Defining mapper precedence

In the example below, the added rule (of associating 111 to PersonEditModel.Id) will just run in case there is no convention rule stating otherwise.

    var repo = new MappingRepository(o =>
        o.MapperOrder
            .Use<ConventionMapper<object, object>>()
            .Then<ManualMapper<object, object>>()
            .Finally<AttributeMapper<object, object>>());

    repo.AddMapping<Person, PersonEditModel>()
        .From(s => 111)
        .To(t => t.Id);

    var mapper = repo.ResolveMapper<Person, PersonEditModel>();

    var result = mapper.Map(source);

### Error handling

When mapping, Moo will wrap all internal exceptions into a MappingException, with details on what mapping it was working on:

    try
    {
        var result = source.MapTo<PersonEditModel>();
    }
    catch (MappingException ohno)
    {
        // Do your exception handling here -- mapping exception will 
		// contain source and target information (their types, 
		// properties being mapped, etc). The Trace code below is just
		// a (bad) example.
        Trace.TraceError(
            "Got an error when mapping. Source: {0}. Target: {1}. Error: {1}",
            ohno.SourceType,
            ohno.TargetType,
            ohno.Message);
    }

### Handling IEnumerable properties

Mappers will be happy to map between properties when they are easily convertible, such as:

* T[] to T[]: as in int[] to int[], string[] to string[], etc
* IEnumerable\<T\> to IEnumerable\<T\>;
* DerivedType[] to BaseType[];
* T[] to IEnumerable<T>;
* All combinations above;

The convention mapper will not automatically convert from int[] to object[]. Internally, the framework does not consider the latter to be assignable from the former.

In case you need an inner mapper for a given IEnumerable property, the syntax below will create and handle one for you:

    MappingRepository.Default
        .AddMapping<Person, PersonDetailsDataContract>()
        .UseMapperFrom(p => p.Contacts)
        .To(pe => pe.PersonContacts);

    var result = source.MapTo<PersonDetailsDataContract>();

Planned/Work in progress
------------------------

### Planned: Set factory methods in Repo (as in repo.CreateObjects.With(t => Activator.CreateInstance(t)).Create<MyClass>.With(() => new MyClass)) 

