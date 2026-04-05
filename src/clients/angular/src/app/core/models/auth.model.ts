export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  expiresAtUtc: string;
  userId: string;
  email: string;
  roles: string[];
}

export interface CurrentUser {
  uid: string;
  email: string;
  roles: string[];
}

export interface ApiErrorBody {
  code?: string;
  message?: string;
}
