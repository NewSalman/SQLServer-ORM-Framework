// cols.Append("{\n");

// for (int i = 0; i < properties.Length; i++)
// {
//     string[] name = properties[i].ToString().Split(' ');

//     if (row[i][name[1]].GetType() == typeof(string) || row[i][name[1]].GetType() == typeof(DateTime))
//     {
//         if (name[1] == properties[properties.Length - 1].ToString().Split(' ')[1])
//         {
//             cols.Append(string.Format("\"{0}\" : \"{1}\"\n", name[1], row[i][name[1]]));
//             break;
//         }

//         cols.Append(string.Format("\"{0}\" : \"{1}\",\n", name[1], row[i][name[1]]));
//     }
//     else
//     {
//         if (name[1] == properties[properties.Length - 1].ToString().Split(' ')[1])
//         {
//             cols.Append(string.Format("\"{0}\" : {1}\n", name[1], row[i][name[1]]));
//             break;
//         }

//         cols.Append(string.Format("\"{0}\" : {1},\n", name[1], row[i][name[1]]));
//     }
// }

// if (row[x] == table.Rows[table.Rows.Count - 1] || table.Rows.Count == 1)
// {
//     cols.Append("}\n");
//     break;
// }
// cols.Append("},\n");