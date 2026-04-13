import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'loading-page',
  imports: [CommonModule],
  templateUrl: './loading-page.component.html',
  styleUrl: './loading-page.component.css',
})
export class LoadingPageComponent {
  @Input() show!: boolean | null;
}
