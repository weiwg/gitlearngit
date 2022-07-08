/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-14 15:54:26
 * @LastEditors: weig
 * @LastEditTime: 2021-11-16 13:47:10
 */

import dayjs from 'dayjs'

/**
 * @description 时间格式化
 * @author weig
 * @param {string | number | Date | Dayjs | null | undefined} date 时间
 * @param {String} pattern 格式文本
 * @returns {Array} list
 */
export function formatTime(date, pattern) {
  return dayjs(date).format(pattern);
}

/**
 * @description 树形结构转列表
 * @author weig
 * @param {Array} tree 树型结构数据
 * @param {String} idValue id值
 * @param {String} childrenField children字段
 * @param {String} idField id字段
 * @param {String} parentIdField parentId字段
 * @returns 
 */
export function treeToList(tree = [], idValue = null, childrenField = 'children', idField = 'id', parentIdField = 'parentId') {
  const list = [];
  if (!childrenField) childrenField = 'children';
  for (let i = 0, j = tree.length; i < j; i++) {
    const d = tree[i];
    const id = d[idField];
    if (!list.some(l => l[idField] === id)) {
      list.push(d);
    }
    if (parentIdField) d[parentIdField] = idValue;
    const children = d[childrenField];
    if (children && children.length > 0) {
      const items = treeToList(children, id, childrenField, idField, parentIdField);
      const values = items.values();
      for (const v of values) {
        if (!list.some(l => l[idField] === v[idField])) {
          list.push(v);
        } 
      }
    }
  }
  return list;
}

/**
 * @description 数据列表转树形结构
 * @author weig
 * @param {Array} list 列表数据
 * @param {Object} root 树根
 * @param {String} idField id字段
 * @param {String} parentIdField 父级id字段
 * @returns 
 */
export function listToTree(list = [], root = null, idField = 'id', parentIdField = 'parentId') {
  const tree = [];
  const hash = {};
  const childrenField = 'children';
  for (let i = 0, l = list.length; i < l; i++) {
    const d = list[i];
    hash[d[idField]] = d;
  }

  for (let i = 0, l = list.length; i < l; i++) {
    const d = list[i];
    const parentID = d[parentIdField];
    if (parentID === '' || parentID === 0) {
      tree.push(d);
      continue;
    }

    const parent = hash[parentID];
    if (!parent) {
      tree.push(d);
      continue;
    }

    let children = parent[childrenField];
    if (!children) {
      children = [];
      parent[childrenField] = children;
    }
    children.push(d);
  }

  if (root) {
    root[childrenField] = tree;
    return [root];
  }

  return tree;
}

/**
 * @description 获取列表父级数据
 * @author weig
 * @param {Array} list 列表集合数据
 * @param {String} idValue 当前id
 * @param {String} idField id字段
 * @param {String} parentIdField 父级id字段
 * @param {Boolean} includeSelf 是否包括自己
 * @returns 
 */
export function getListParents(list = [], idValue, idField = 'id', parentIdField = 'parentId', includeSelf = false) {
  const parents = [];
  const self = list.find(o => o[idField] === idValue);
  if (!self) {
    return parents;
  }

  if (includeSelf) {
    parents.unshift(self);
  }

  let parent = list.find(o => o[idField] === self[parentIdField]);
  while (parent && (parent[idField] != 0 || parent[idField] != "")) {
    parents.unshift(parent);
    parent = list.find(o => o[idField] === parent[parentIdField]);
  }
  return parents;
}

/**
 * @description 获取列表父级数据包括自己
 */
export function getListParent(list = [], idValue, idField = 'id', parentIdField = 'parentId', includeSelf = true) {
  const parents = [];
  const self = list.find(o => o[idField] === idValue);
  if (!self) {
    return parents;
  }

  if (includeSelf) {
    parents.unshift(self);
  }

  let parent = list.find(o => o[idField] === self[parentIdField]);
  while (parent && (parent[idField] != 0 || parent[idField] != "")) {
    parents.unshift(parent);
    parent = list.find(o => o[idField] === parent[parentIdField]);
  }
  return parents;
}
/**
 * @description 获取树结构父级数据
 * @author weig
 * @param {Array} tree 树形结构数据
 * @param {String} idValue 当前id
 * @param {String} childrenField 子级字段
 * @param {String} idField id字段
 * @param {String} parentIdField 父级id字段
 * @param {String} parentIdValue 父级id的值
 * @returns 
 */
export function getTreeParents(tree = [], idValue, childrenField = 'children', idField = 'id', parentIdField = 'parentId', parentIdValue = 0) {
  const list = treeToList(tree, parentIdValue, childrenField, idField, parentIdField);
  return getListParents(list, idValue, idField, parentIdField);
}

/**
 * @description 获取树结构父级数据以及个人
 * @returns 
 */
export function getTreeParent(tree = [], idValue, childrenField = 'children', idField = 'id', parentIdField = 'parentId', parentIdValue = 0) {
  const list = treeToList(tree, parentIdValue, childrenField, idField, parentIdField);
  return getListParent(list, idValue, idField, parentIdField);
}
/**
 * @description 通过自己获取树形结构数据中的所有父级
 * @author weig
 * @param {Array} tree 树形结构数据
 * @param {String} idValue 当前id
 * @param {String} childrenField 子级字段
 * @param {String} idField id字段
 * @param {String} parentIdField 父级id字段
 * @param {String} parentIdValue 父级id的值
 * @returns 
 */
export function getTreeParentsWithSelf(tree = [], idValue, childrenField = 'children', idField = 'id', parentIdField = 'parentId', parentIdValue = 0) {
  const list = treeToList(tree, parentIdValue, childrenField, idField, parentIdField);
  return getListParents(list, idValue, idField, parentIdField, true);
}
