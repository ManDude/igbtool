#pragma once

inline int align(u64 off, int al) {
  int res = off + (al - 1);
  return res - res % al;
}
