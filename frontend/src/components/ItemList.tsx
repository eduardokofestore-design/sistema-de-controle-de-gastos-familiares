import type { Key, ReactNode } from 'react';

type ItemListProps<T> = {
  title: string;
  items: T[];
  emptyMessage: string;
  getKey: (item: T) => Key;
  renderItem: (item: T) => ReactNode;
};

export function ItemList<T>({ title, items, emptyMessage, getKey, renderItem }: ItemListProps<T>) {
  return (
    <section className="panel">
      <h2>{title}</h2>
      {items.length === 0 ? (
        <p className="empty-state">{emptyMessage}</p>
      ) : (
        <ul className="list">
          {items.map((item) => (
            <li key={getKey(item)} className="list-item">
              {renderItem(item)}
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
