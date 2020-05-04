# UncommonSense.Bc.Utils

## Object ID Availability
The advent of the so called modern development experience for Microsoft Dynamics 365 Business Central has not necessarily made it easier to find available IDs for new objects. The functions below form building blocks to find available IDs, but can also be used separately to retrieve relevant information.

### Object IDs in Use: Get-BcObjectInfo
Retrieves object information (Type, ID, Name and, where applicable, BaseName ) from AL objects that reside in disk files.

#### Parameters
| Name       | Description                        | Mandatory | Default Value        |
| ---------- | ---------------------------------- | --------- | -------------------- |
| Path       | The path to the folder to examine  | No        | Current folder (`.`) |
| ObjectType | The object types to return         | No        | All object types     |
| Recurse    | Also consider subfolders of `Path` | No        | No                   |

### Object IDs in Use/Reserved Object IDs: New-BcObjectIdInfo
Creates output objects that each contain a combination of an object type and object ID. In the ID availability functions below, these output objects can be used e.g. to represent objects that are reserved or in use.

#### Parameters
| Name       | Description            | Mandatory | Default Value |
| ---------- | ---------------------- | --------- | ------------- |
| ObjectType | The type of the object | Yes       | (none)        |
| ObjectID   | The ID of the object   | Yes       | (none)        |

### Valid ID Ranges: Get-BcObjectIdRange
Retrieves the object ID ranges from an app's `app.json` file.

> Note that the ranges in `app.json` (by definition) apply to all object types. You can reduce the output to the object types you're interested in using the ObjectType parameter, as described below.

#### Parameters
| Name       | Description                        | Mandatory | Default Value        |
| ---------- | ---------------------------------- | --------- | -------------------- |
| Path       | The path that contains `app.json`  | No        | Current folder (`.`) |
| ObjectType | The types to include in the output | No        | All object types     |

