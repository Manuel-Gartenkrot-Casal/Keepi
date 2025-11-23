import { cookies } from "next/headers"
import type { Usuario } from "./db"

export interface SessionData {
  usuario: Usuario
  nombreHeladera: string
}

export async function getSession(): Promise<SessionData | null> {
  const cookieStore = await cookies()
  const sessionCookie = cookieStore.get("keepi_session")

  if (!sessionCookie) {
    return null
  }

  try {
    return JSON.parse(sessionCookie.value)
  } catch {
    return null
  }
}

export async function setSession(data: SessionData) {
  const cookieStore = await cookies()
  cookieStore.set("keepi_session", JSON.stringify(data), {
    httpOnly: true,
    secure: process.env.NODE_ENV === "production",
    sameSite: "lax",
    maxAge: 60 * 60 * 24 * 7, // 7 days
  })
}

export async function clearSession() {
  const cookieStore = await cookies()
  cookieStore.delete("keepi_session")
}
