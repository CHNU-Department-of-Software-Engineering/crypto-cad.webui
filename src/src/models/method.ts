import {
  TYPE_CIPHER,
  TYPE_HASH,
  OPERATION_DECRYPTION,
  OPERATION_ENCRYPTION,
  RELATION_CHILD,
  RELATION_PARENT
} from '@/constants/method'

export type MethodType = typeof TYPE_CIPHER | typeof TYPE_HASH
export type OperationType = typeof OPERATION_DECRYPTION | typeof OPERATION_ENCRYPTION
export type MethodRelation = typeof RELATION_CHILD | typeof RELATION_PARENT

export interface MethodConfiguration {
  [key: string]: number[]
}
export interface Method {
  id: string,
  name: string,
  type: MethodType,
  family: string,
  isModifiable: boolean,
  relation: MethodRelation,
  secretLength: number,
  configuration: MethodConfiguration
}
