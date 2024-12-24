// Application.DTOs.Create
export interface NoteCreate {
  title: string;
  content: string;
}

export interface RegisterDto {
  username: string;
  plainPassword: string;
}

// Application.DTOs.Request
export interface LoginDto {
  username: string;
  plainPassword: string;
}

export interface NoteRequest {
  id: number;
}

// Application.DTOs.Response
export interface LoginResponse {
  jwt: string;
}

export interface NoteResponse {
  id: number;
  title: string;
  content: string;
}

// Application.DTOs.Update
export interface NoteUpdate {
  id: number;
  title: string;
  content: string;
}

export interface UserUpdate {
  username: string;
}
