#pragma once

#include "common/types.h"

#include <vector>
#include <string>
#include <functional>

struct IgbSectionHeader {
  u32 size;
  u32 count;
};

struct IgbHeader {
  IgbSectionHeader sections[5];
  u32 magic;
  u32 version;
};

struct IgbType {
  std::string name;
  bool u_bool;
  // u32 u_val;
  u32 name_length;
  int size = -4;
  bool is_array = false;
  bool dynamic = false;
  const IgbType* elt_type = nullptr;
};

struct IgbShaderSymbol {
  std::string name;
};

struct IgbStructDef {
  u32 name_length;
  u32 is_master;
  u32 u_val1;
  u32 field_count;
  s32 parent;
  u32 u_val2;
};

struct IgbStructField {
  const IgbType* type;
  u16 unk;
  u16 size;
};

struct IgbStruct {
  int id;
  std::string name;
  u32 is_master;
  // u32 u_val1;
  u32 field_count;
  s32 parent;
  u32 u_val2;
  std::vector<IgbStructField> fields;
};

struct IgbRef {
  // s32 id;

  u32 type;
  u32 size;
  u32 unk;
};

enum class IgbRefKind { UNKNOWN, OBJECT, DATA, NONE };
struct IgbRefInfoExt {
  IgbRefKind type;
  int index;
  IgbRefInfoExt(IgbRefKind type, int index) : type(type), index(index){};
};

struct IgbObjectRef : IgbRef {
  u32 struct_id;
};

struct IgbStaticRef : IgbRef {
  u32 data_size;
  u32 data_type;
  u32 unk2;
  s32 unk3;
};
static_assert(sizeof(IgbStaticRef) == sizeof(IgbObjectRef) + 12);

struct IgbObject {
  int ref_idx;
  IgbStruct* obj_struct;
  // u32 unk;
  std::vector<u8> data;
};

struct IgbStaticData {
  int ref_idx;
  IgbType* type;
  u32 unk1;
  u32 unk2;
  s32 unk3;
  std::vector<u8> data;
};

enum IgbTypeInfoFlags { DYNAMIC = 0x1 };
class IgbTypeInfo;
std::string type_info_default_func(const IgbTypeInfo&, const u8*);
class IgbTypeInfo {
  std::function<std::string(const IgbTypeInfo&, const u8*)> print_func;

  int size;
  void guess_size();

 public:
  IgbTypeInfo(std::string name, const std::string format)
      : name(name), default_format(format), print_func(type_info_default_func), size(-4) {
    guess_size();
  };
  IgbTypeInfo(std::string name, std::function<std::string(const IgbTypeInfo&, const u8*)> func)
      : name(name), default_format(""), print_func(func), size(-4) {
    guess_size();
  };
  IgbTypeInfo(std::string name, int sz) : name(name), default_format(""), print_func(type_info_default_func), size(sz){};
  IgbTypeInfo(std::string name, int sz, const std::string format)
      : name(name), default_format(format), print_func(type_info_default_func), size(sz){};
  IgbTypeInfo(std::string name, const std::string format, IgbTypeInfoFlags flags)
      : name(name), default_format(format), print_func(type_info_default_func), size(-4) {
    guess_size();
    if (flags & IgbTypeInfoFlags::DYNAMIC) {
      dynamic = true;
    }
  };

  const std::string name;
  const std::string default_format;
  bool dynamic = false;

  std::string print(const u8*) const;
  std::string print_no_name(const u8*) const;

  std::vector<std::string> parse_format() const;

  int get_size() const { return size; }
};

class IgbFile {
  std::vector<IgbType> types;
  std::vector<IgbShaderSymbol> shader_symbols;
  u32 shader_val;
  std::vector<IgbStruct> structs;
  std::vector<IgbRefInfoExt> ref_idx;
  u32 top_object;
  std::vector<IgbObject> objs;
  std::vector<IgbStaticData> static_data;

  int get_type_size(const IgbType* type, const u8* data) const;
  int get_field_size(const IgbStructField& field, const u8* data) const;

  u32 version;

  void print_graph_child(std::string& str,
                         std::vector<int>& id_map,
                         std::vector<int>& id_stack,
                         int id,
                         int level,
                         std::vector<bool>& child_bits,
                         bool print_child,
                         bool recursive_warn);

 public:
  IgbFile(const std::vector<u8>& data);

  std::string print_types();
  std::string print_structs();
  std::string print_refs();
  std::string print_objects();
  std::string print_data();
  std::string print_misc();
  std::string print_graph();

  std::string print_type_data(const IgbType* type, const u8* data, bool noname = false) const;
  std::string print_field_data(const IgbStructField& field, const u8* data) const;

  IgbRefKind get_ref_kind(int idx) const;
  const IgbObject* get_object_from_ref(int idx);
  const IgbStaticData* get_data_from_ref(int idx);

  const IgbType* get_type_by_name(const std::string& name) const;

  void RipImages();
};

void igb_init_globals();
