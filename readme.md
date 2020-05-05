# UncommonSense.Bc.Utils

## Object ID Availability
The advent of the so called modern development experience for Microsoft Dynamics 365 Business Central has not necessarily made it easier to find available IDs for new objects. **This module assumes that available IDs can be found by taking an app's allotted ID range, and subtracting any IDs that are either reserved or already in use**. The cmdlets below form building blocks to find available IDs, but can also be used separately to retrieve relevant information.

### Object IDs in Use: Get-BcObjectInfo
Retrieves object information (Type, ID, Name and, where applicable, BaseName ) from AL objects that reside in disk files.

#### Parameters
| Name       | Description                        | Mandatory | Default Value        |
| ---------- | ---------------------------------- | --------- | -------------------- |
| Path       | The path to the folder to examine  | No        | Current folder (`.`) |
| ObjectType | The object types to return         | No        | All object types     |
| Recurse    | Also consider subfolders of `Path` | No        | No                   |

### Object IDs in Use/Reserved Object IDs: New-BcObjectIdInfo
Creates output objects that each contain a combination of an object type and object ID. In the ID availability cmdlets below, these output objects can be used e.g. to represent objects that are reserved or simulate IDs that are in use.

#### Parameters
| Name       | Description            | Mandatory | Default Value |
| ---------- | ---------------------- | --------- | ------------- |
| ObjectType | The type of the object | Yes       | (none)        |
| ObjectID   | The ID of the object   | Yes       | (none)        |

### Valid ID Ranges: Get-BcObjectIdRange
Retrieves the object ID ranges from an app's `app.json` file, given the app's folder path.

> Note that the ranges in `app.json` (by definition) apply to all object types. You can reduce the output to just the object type(s) you're interested in using the `-ObjectType` parameter, as described below.

#### Parameters
| Name       | Description                        | Mandatory | Default Value        |
| ---------- | ---------------------------------- | --------- | -------------------- |
| Path       | The path that contains `app.json`  | No        | Current folder (`.`) |
| ObjectType | The types to include in the output | No        | All object types     |

### Valid ID Ranges: New-BCObjectIdRange
In cases where the ID ranges in `app.json` don't suffice (or no such file is available), New-BcObjectIdRange can be used to define available ID ranges.

#### Parameters
| Name         | Description                           | Mandatory | Default Value |
| ------------ | ------------------------------------- | --------- | ------------- |
| ObjectType   | The object type to create a range for | Yes       | (none)        |
| FromObjectID | The lower boundary of the ID range    | Yes       | (none)        |
| ToObjectID   | The upper boundary of the ID range    | Yes       | (none)        |

### Calculating ID Availability: Get-BcObjectIdAvailability
Building on the previous cmdlets, calculates the availability state (Available, Reserved or InUse) of the objects in a folder or folder structure.

#### Parameters
| Name       | Description                                    | Mandatory | Default Value                          |
| ---------- | ---------------------------------------------- | --------- | -------------------------------------- |
| Path       | The path to the folder to examine              | Yes       | Current folder (`.`)                   |
| Recurse    | Also consider subfolders of `Path`             | No        | No                                     |
| ObjectType | The object type(s) to limit the analysis to    | No        | All object types                       |
| IdRange    | A scriptblock that returns objects in use      | No        | Calls `Get-BcObjectIdRange` for `Path` |
| Reserved   | A scriptblock that returns reserved object IDs | No        | An empty array                         |
| InUse      | A scriptblock that returns object IDs in use   | No        | Calls `Get-BcObjectInfo` for `Path`    |
| Summary    | Output per ID, instead of per Type + ID        | No        | No                                     |