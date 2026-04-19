export interface QuantityDTO {
  value: number;
  unit: string;
}

export interface ComparisonRequest {
  first: QuantityDTO;
  second: QuantityDTO;
}

export interface ComparisonResponse {
  isEqual: boolean;
  message: string;
}

export interface ArithmeticRequest {
  first: QuantityDTO;
  second: QuantityDTO;
  targetUnit?: string;
}

export interface ConvertRequest {
  value: number;
  unit: string;
  targetUnit: string;
}

export interface DivideResponse {
  ratio: number;
}

export interface MultiplyResponse {
  productOfBaseValues: number;
}

export interface UnitsResponse {
  [category: string]: string[];
}

export interface LoginRequest {
  email: string;
  password?: string;
}

export interface LoginResponse {
  message: string;
  token: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password?: string;
}

export interface RegisterResponse {
  message: string;
}

export interface ActivityLog {
  timestamp: Date;
  operation: string;
  category: string;
  input: string;
  result: string;
}
