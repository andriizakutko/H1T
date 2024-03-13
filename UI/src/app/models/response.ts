export interface ApiResponse<T> {
    statusCode: number,
    data: T,
    error: Error
}

interface Error {
    code: string,
    message: string
}