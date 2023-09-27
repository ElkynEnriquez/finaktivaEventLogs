export interface IJsonPatch {
  path: string,
  op: "replace" | "remove" | "add",
  value: any,
}
