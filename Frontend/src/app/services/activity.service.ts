import { Injectable, signal } from '@angular/core';
import { ActivityLog } from '../models';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  private readonly STORAGE_KEY = 'measurement_activity_history';
  private activities = signal<ActivityLog[]>([]);

  constructor() {
    this.loadFromStorage();
  }

  getActivities() {
    return this.activities.asReadonly();
  }

  addActivity(activity: Omit<ActivityLog, 'timestamp'>) {
    const newActivity: ActivityLog = {
      ...activity,
      timestamp: new Date()
    };
    
    const current = this.activities();
    const updated = [newActivity, ...current].slice(0, 50); // Keep last 50
    this.activities.set(updated);
    this.saveToStorage(updated);
  }

  clearActivities() {
    this.activities.set([]);
    localStorage.removeItem(this.STORAGE_KEY);
  }

  private saveToStorage(data: ActivityLog[]) {
    if (typeof window !== 'undefined') {
      localStorage.setItem(this.STORAGE_KEY, JSON.stringify(data));
    }
  }

  private loadFromStorage() {
    if (typeof window !== 'undefined') {
      const stored = localStorage.getItem(this.STORAGE_KEY);
      if (stored) {
        try {
          const parsed = JSON.parse(stored);
          this.activities.set(parsed.map((a: any) => ({
            ...a,
            timestamp: new Date(a.timestamp)
          })));
        } catch (e) {
          console.error('Failed to parse activities from storage', e);
        }
      }
    }
  }
}
